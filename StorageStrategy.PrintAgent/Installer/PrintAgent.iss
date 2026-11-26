; Instalador do agente de impressão do StorageStrategy (feature 006).
; Gere com: StorageStrategy.PrintAgent\Installer\build-installer.ps1
; Saída em StorageStrategy_API\installer-dist\ (fora do Git).
;
; Fluxo: escolher a impressora -> login de um GERENTE -> o agente (--setup) cadastra a estação na API e grava a
; configuração -> instala e inicia o serviço do Windows -> confirma a conexão (--verify) -> imprime o papel de teste.
;
; Para testes: setup.exe /API=http://localhost:11800/api/

#define AppName "StorageStrategy - Agente de Impressão"
#ifndef AppVersion
  #define AppVersion "1.0.0"
#endif
#ifndef PublishDir
  #define PublishDir "..\..\installer-dist\publish"
#endif
#define ServiceName "StorageStrategyPrintAgent"
#define ExeName "StorageStrategy.PrintAgent.exe"
#define DefaultApiUrl "https://api.storagestrategy.softwareselitegate.com.br/api/"

[Setup]
AppId={{8E0C7A52-4B7E-4F7B-9B5E-2C1F6D3A9E41}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher=StorageStrategy
DefaultDirName={autopf}\StorageStrategy\PrintAgent
DisableDirPage=yes
DisableProgramGroupPage=yes
DisableReadyPage=no
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
MinVersion=10.0
OutputBaseFilename=StorageStrategy-Impressora-Setup-{#AppVersion}
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
UninstallDisplayName={#AppName}
UninstallDisplayIcon={app}\{#ExeName}
CloseApplications=no

[Languages]
Name: "ptbr"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Files]
Source: "{#PublishDir}\{#ExeName}"; DestDir: "{app}"; Flags: ignoreversion

[UninstallDelete]
Type: files; Name: "{app}\appsettings.json"

[Code]
var
  PrinterPage: TWizardPage;
  PrinterCombo: TNewComboBox;
  PrinterPortLabel: TNewStaticText;
  PrinterNames: TArrayOfString;
  PrinterPorts: TArrayOfString;
  LoginPage: TInputQueryWizardPage;
  KeepConfig: Boolean;
  CompanyName: String;
  StationReplaced: Boolean;
  FinalMessage: String;

{ ---------- utilidades ---------- }

function ApiUrl: String;
begin
  Result := ExpandConstant('{param:API|{#DefaultApiUrl}}');
end;

function ConfigPath: String;
begin
  Result := WizardDirValue + '\appsettings.json';
end;

function RunSc(const Params: String): Integer;
var
  Code: Integer;
begin
  if not Exec(ExpandConstant('{sys}\sc.exe'), Params, '', SW_HIDE, ewWaitUntilTerminated, Code) then
    Code := -1;
  Result := Code;
end;

function ServiceExists: Boolean;
begin
  Result := RunSc('query {#ServiceName}') = 0;
end;

{ Lê "chave=valor" do arquivo de resultado gravado pelo agente. }
function ResultValue(const FileName, Key: String): String;
var
  Lines: TArrayOfString;
  I: Integer;
begin
  Result := '';
  if not LoadStringsFromFile(FileName, Lines) then
    Exit;
  for I := 0 to GetArrayLength(Lines) - 1 do
    if Pos(Key + '=', Lines[I]) = 1 then
    begin
      Result := Copy(Lines[I], Length(Key) + 2, MaxInt);
      Exit;
    end;
end;

function RunAgent(const ExePath, Params, ResultFile: String; var Message: String): Integer;
var
  Code: Integer;
begin
  DeleteFile(ResultFile);
  if not Exec(ExePath, Params + ' --result "' + ResultFile + '"', '', SW_HIDE, ewWaitUntilTerminated, Code) then
  begin
    Message := 'Não foi possível executar o agente de impressão.';
    Result := -1;
    Exit;
  end;
  Message := ResultValue(ResultFile, 'message');
  if Message = '' then
    Message := 'O agente terminou sem informar o resultado (código ' + IntToStr(Code) + ').';
  Result := Code;
end;

function IsLocalPort(const Port: String): Boolean;
var
  P: String;
begin
  P := Uppercase(Trim(Port));
  Result := (Pos('USB', P) = 1) or (Pos('COM', P) = 1) or (Pos('LPT', P) = 1) or
            (Pos('BTH', P) = 1) or (Pos('TMUSB', P) = 1) or (Pos('ESDPRT', P) = 1);
end;

function LooksThermal(const Name: String): Boolean;
var
  N: String;
begin
  N := Uppercase(Name);
  Result := (Pos('HPRT', N) > 0) or (Pos('MPT', N) > 0) or (Pos('POS', N) > 0) or (Pos('58', N) > 0) or
            (Pos('TERMIC', N) > 0) or (Pos('THERMAL', N) > 0) or (Pos('ELGIN', N) > 0) or
            (Pos('BEMATECH', N) > 0) or (Pos('EPSON TM', N) > 0) or (Pos('KNUP', N) > 0);
end;

{ ---------- impressoras do Windows (WMI) ---------- }

procedure LoadPrinters;
var
  Locator, Service, Items, Item: Variant;
  I, Count, Best: Integer;
begin
  SetArrayLength(PrinterNames, 0);
  SetArrayLength(PrinterPorts, 0);
  try
    Locator := CreateOleObject('WbemScripting.SWbemLocator');
    Service := Locator.ConnectServer('.', 'root\CIMV2');
    Items := Service.ExecQuery('SELECT Name, PortName FROM Win32_Printer');
    Count := Items.Count;
    SetArrayLength(PrinterNames, Count);
    SetArrayLength(PrinterPorts, Count);
    for I := 0 to Count - 1 do
    begin
      Item := Items.ItemIndex(I);
      PrinterNames[I] := Item.Name;
      PrinterPorts[I] := Item.PortName;
    end;
  except
    SetArrayLength(PrinterNames, 0);
    SetArrayLength(PrinterPorts, 0);
  end;

  PrinterCombo.Items.Clear;
  Best := -1;
  for I := 0 to GetArrayLength(PrinterNames) - 1 do
  begin
    PrinterCombo.Items.Add(PrinterNames[I]);
    if (Best < 0) and LooksThermal(PrinterNames[I]) and IsLocalPort(PrinterPorts[I]) then
      Best := I;
  end;
  if (Best < 0) then
    for I := 0 to GetArrayLength(PrinterNames) - 1 do
      if (Best < 0) and LooksThermal(PrinterNames[I]) then
        Best := I;
  PrinterCombo.ItemIndex := Best;
end;

procedure UpdatePortLabel(Sender: TObject);
var
  I: Integer;
begin
  I := PrinterCombo.ItemIndex;
  if I < 0 then
    PrinterPortLabel.Caption := 'Nenhuma impressora selecionada.'
  else if IsLocalPort(PrinterPorts[I]) then
    PrinterPortLabel.Caption := 'Porta: ' + PrinterPorts[I] + '  (ok)'
  else
    PrinterPortLabel.Caption := 'Porta: ' + PrinterPorts[I] + #13#10 +
      'ATENÇÃO: esta porta não é USB nem COM (Bluetooth). Nessa porta o papel não sai na impressora.';
end;

procedure RefreshClick(Sender: TObject);
begin
  LoadPrinters;
  UpdatePortLabel(nil);
end;

{ ---------- páginas ---------- }

procedure InitializeWizard;
var
  Intro: TNewStaticText;
  Refresh: TNewButton;
begin
  KeepConfig := False;

  PrinterPage := CreateCustomPage(wpWelcome, 'Impressora',
    'Escolha a impressora térmica que vai imprimir as comandas.');

  Intro := TNewStaticText.Create(PrinterPage);
  Intro.Parent := PrinterPage.Surface;
  Intro.AutoSize := False;
  Intro.WordWrap := True;
  Intro.Width := PrinterPage.SurfaceWidth;
  Intro.Height := ScaleY(44);
  Intro.Caption := 'A impressora precisa estar instalada no Windows (driver do fabricante) e ligada por USB ou ' +
    'pareada por Bluetooth. Se ela não aparecer na lista, instale o driver e clique em "Atualizar lista".';

  PrinterCombo := TNewComboBox.Create(PrinterPage);
  PrinterCombo.Parent := PrinterPage.Surface;
  PrinterCombo.Style := csDropDownList;
  PrinterCombo.Top := Intro.Top + Intro.Height + ScaleY(8);
  PrinterCombo.Width := PrinterPage.SurfaceWidth - ScaleX(110);
  PrinterCombo.OnChange := @UpdatePortLabel;

  Refresh := TNewButton.Create(PrinterPage);
  Refresh.Parent := PrinterPage.Surface;
  Refresh.Caption := 'Atualizar lista';
  Refresh.Top := PrinterCombo.Top - ScaleY(1);
  Refresh.Left := PrinterCombo.Width + ScaleX(10);
  Refresh.Width := ScaleX(100);
  Refresh.Height := ScaleY(25);
  Refresh.OnClick := @RefreshClick;

  PrinterPortLabel := TNewStaticText.Create(PrinterPage);
  PrinterPortLabel.Parent := PrinterPage.Surface;
  PrinterPortLabel.AutoSize := False;
  PrinterPortLabel.WordWrap := True;
  PrinterPortLabel.Top := PrinterCombo.Top + PrinterCombo.Height + ScaleY(10);
  PrinterPortLabel.Width := PrinterPage.SurfaceWidth;
  PrinterPortLabel.Height := ScaleY(48);

  LoginPage := CreateInputQueryPage(PrinterPage.ID, 'Acesso ao StorageStrategy',
    'Entre com o login de um GERENTE da empresa.',
    'O instalador usa este login só para cadastrar a impressora da empresa. A senha não é gravada neste computador.');
  LoginPage.Add('E-mail do gerente:', False);
  LoginPage.Add('Senha:', True);
  LoginPage.Add('Nome desta impressora no sistema:', False);
  LoginPage.Values[2] := 'Caixa - ' + GetComputerNameString;

  LoadPrinters;
  UpdatePortLabel(nil);
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  Result := KeepConfig and ((PageID = PrinterPage.ID) or (PageID = LoginPage.ID));
end;

procedure CurPageChanged(CurPageID: Integer);
begin
  if CurPageID = wpFinished then
    WizardForm.FinishedLabel.Caption := FinalMessage;
end;

{ Atualização: já existe configuração neste computador? Oferece manter e só trocar o programa. }
procedure AskKeepConfig;
begin
  KeepConfig := FileExists(ConfigPath) and (MsgBox('Este computador já tem o agente de impressão configurado.' + #13#10#13#10 +
    'Deseja manter a configuração atual e só atualizar o programa?' + #13#10 +
    '(Escolha "Não" para refazer a configuração com o login do gerente.)', mbConfirmation, MB_YESNO) = IDYES);
end;

function ValidatePrinterPage: Boolean;
var
  I: Integer;
begin
  Result := False;
  I := PrinterCombo.ItemIndex;
  if I < 0 then
  begin
    MsgBox('Selecione a impressora térmica.', mbError, MB_OK);
    Exit;
  end;
  if not IsLocalPort(PrinterPorts[I]) then
  begin
    if MsgBox('A impressora "' + PrinterNames[I] + '" está na porta "' + PrinterPorts[I] + '".' + #13#10#13#10 +
      'Essa porta não é USB nem COM (Bluetooth): o papel provavelmente NÃO vai sair na impressora ' +
      '(ex.: "Cloud TCP/IP Port" envia para a nuvem do fabricante).' + #13#10#13#10 +
      'Corrija em Configurações do Windows > Impressoras > Propriedades da impressora > Portas.' + #13#10#13#10 +
      'Continuar mesmo assim?', mbError, MB_YESNO or MB_DEFBUTTON2) <> IDYES then
      Exit;
  end;
  Result := True;
end;

function RunSetup: Boolean;
var
  Exe, Cred, Config, ResultFile, Message, Params: String;
  Code: Integer;
  Secret: TArrayOfString;
begin
  Result := False;

  if (Trim(LoginPage.Values[0]) = '') or (LoginPage.Values[1] = '') then
  begin
    MsgBox('Informe o e-mail e a senha do gerente.', mbError, MB_OK);
    Exit;
  end;

  WizardForm.NextButton.Enabled := False;
  WizardForm.BackButton.Enabled := False;
  try
    ExtractTemporaryFile('{#ExeName}');
    Exe := ExpandConstant('{tmp}\{#ExeName}');
    Cred := ExpandConstant('{tmp}\cred.txt');
    Config := ExpandConstant('{tmp}\appsettings.json');
    ResultFile := ExpandConstant('{tmp}\setup-result.txt');

    { Senha vai por arquivo UTF-8 na pasta temporária do instalador (nunca na linha de comando); o agente apaga ao ler. }
    SetArrayLength(Secret, 1);
    Secret[0] := LoginPage.Values[1];
    if not SaveStringsToUTF8File(Cred, Secret, False) then
    begin
      MsgBox('Não foi possível preparar o acesso. Tente de novo.', mbError, MB_OK);
      Exit;
    end;

    Params := '--setup --api "' + ApiUrl + '" --email "' + Trim(LoginPage.Values[0]) + '"' +
      ' --password-file "' + Cred + '"' +
      ' --printer "' + PrinterNames[PrinterCombo.ItemIndex] + '"' +
      ' --station-name "' + Trim(LoginPage.Values[2]) + '"' +
      ' --config "' + Config + '"';

    Code := RunAgent(Exe, Params, ResultFile, Message);
    DeleteFile(Cred);

    if Code <> 0 then
    begin
      MsgBox(Message, mbError, MB_OK);
      Exit;
    end;

    CompanyName := ResultValue(ResultFile, 'company');
    StationReplaced := ResultValue(ResultFile, 'replaced') = '1';

    if StationReplaced then
      MsgBox('Empresa: ' + CompanyName + #13#10#13#10 +
        'A empresa já tinha uma impressora cadastrada. Uma chave nova foi gerada: o computador antigo ' +
        'deixa de imprimir e este passa a ser o caixa da impressão.', mbInformation, MB_OK)
    else
      MsgBox('Empresa: ' + CompanyName + #13#10#13#10 + 'Impressora cadastrada com sucesso.', mbInformation, MB_OK);

    Result := True;
  finally
    WizardForm.NextButton.Enabled := True;
    WizardForm.BackButton.Enabled := True;
  end;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;
  if CurPageID = wpWelcome then
  begin
    AskKeepConfig;
    Exit;
  end;
  if KeepConfig then
    Exit;
  if CurPageID = PrinterPage.ID then
    Result := ValidatePrinterPage
  else if CurPageID = LoginPage.ID then
    Result := RunSetup;
end;

{ ---------- instalação do serviço ---------- }

function PrepareToInstall(var NeedsRestart: Boolean): String;
begin
  Result := '';
  if ServiceExists then
  begin
    RunSc('stop {#ServiceName}');
    Sleep(3000);
  end;
end;

procedure InstallService;
var
  BinPath: String;
begin
  BinPath := 'binPath= "\"' + ExpandConstant('{app}\{#ExeName}') + '\""';
  if ServiceExists then
    RunSc('config {#ServiceName} ' + BinPath + ' start= auto')
  else
    RunSc('create {#ServiceName} ' + BinPath + ' start= auto DisplayName= "StorageStrategy - Agente de Impressao"');
  RunSc('description {#ServiceName} "Imprime as comandas do StorageStrategy na impressora termica do caixa."');
  RunSc('failure {#ServiceName} reset= 86400 actions= restart/5000/restart/5000/restart/30000');
  RunSc('start {#ServiceName}');
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  Exe, ResultFile, Message, Printer: String;
begin
  if CurStep <> ssPostInstall then
    Exit;

  if not KeepConfig then
    CopyFile(ExpandConstant('{tmp}\appsettings.json'), ConfigPath, False);

  WizardForm.StatusLabel.Caption := 'Iniciando o serviço de impressão...';
  InstallService;
  Sleep(3000);

  Exe := ExpandConstant('{app}\{#ExeName}');
  ResultFile := ExpandConstant('{tmp}\verify-result.txt');

  WizardForm.StatusLabel.Caption := 'Conferindo a conexão com o StorageStrategy...';
  if RunAgent(Exe, '--verify --config "' + ConfigPath + '"', ResultFile, Message) <> 0 then
  begin
    FinalMessage := 'O agente foi instalado, mas a conexão não foi confirmada:' + #13#10 + Message + #13#10#13#10 +
      'Verifique a internet. O serviço tenta de novo sozinho.';
    Exit;
  end;
  if CompanyName = '' then
    CompanyName := ResultValue(ResultFile, 'company');

  if not KeepConfig then
  begin
    Printer := PrinterNames[PrinterCombo.ItemIndex];
    WizardForm.StatusLabel.Caption := 'Imprimindo o papel de teste...';
    if RunAgent(Exe, '--print-check --printer "' + Printer + '" --company "' + CompanyName + '"',
      ExpandConstant('{tmp}\print-result.txt'), Message) <> 0 then
    begin
      FinalMessage := 'Conectado à empresa ' + CompanyName + ', mas o papel de teste falhou:' + #13#10 + Message;
      Exit;
    end;
  end;

  FinalMessage := 'Pronto! A impressora está conectada à empresa ' + CompanyName + '.' + #13#10#13#10 +
    'Confira se saiu o papel "INSTALAÇÃO CONCLUÍDA". A partir de agora, o botão Imprimir da comanda ' +
    '(no caixa ou no celular) imprime nesta impressora.';
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then
  begin
    RunSc('stop {#ServiceName}');
    Sleep(2000);
    RunSc('delete {#ServiceName}');
  end;
end;
