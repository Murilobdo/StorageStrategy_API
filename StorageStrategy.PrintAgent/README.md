# StorageStrategy.PrintAgent

Agente de impressão que roda no **computador do caixa**, ao lado da impressora térmica USB (MP58C7 /
MPT-II, 58mm). Ele busca na API os pedidos de impressão da empresa e envia para a impressora.
Fluxo completo: `specs/006-order-thermal-print/` (diagrama, contratos e quickstart).

## Instalador (recomendado)

O jeito normal de instalar no cliente é o **setup.exe**: ele faz login com um gerente, cadastra a impressora na
API, instala o serviço e imprime um papel de teste. Para gerar:

```powershell
.\StorageStrategy.PrintAgent\Installer\build-installer.ps1 -Version 1.0.0   # requer Inno Setup 6
```

Saída em `installer-dist\` (fora do Git). Roteiro completo: `specs/006-order-thermal-print/deploy-runbook.md`.

Modos do executável usados pelo instalador: `--setup`, `--verify`, `--print-check` (ver `Setup/AgentSetup.cs`).

O restante deste arquivo é a **instalação manual**, para diagnóstico.

## 1. Pré-requisitos

- Windows 10/11 x64.
- Impressora instalada no Windows (ex.: `HPRT MPT-II` ou `POS-58`). Confira o nome exato em
  *Configurações → Bluetooth e dispositivos → Impressoras* ou com `Get-Printer` no PowerShell.
- Impressora cadastrada no Site (*Configurações → Impressora do Caixa*) e a **chave da estação** copiada
  (ela aparece uma única vez; se perder, use "Gerar nova chave").

## 2. Publicar

Na máquina de desenvolvimento, dentro de `StorageStrategy_API`:

```powershell
dotnet publish StorageStrategy.PrintAgent -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o .\publish\PrintAgent
```

Copie a pasta `publish\PrintAgent` para o computador do caixa (ex.: `C:\StorageStrategy\PrintAgent`).

## 3. Configurar

Edite `appsettings.json` na pasta copiada (nunca versione a chave):

```json
"PrintAgent": {
  "ApiBaseUrl": "https://SEU-HOST-DA-API/api/",
  "StationKey": "ps_...",
  "PrinterName": "HPRT MPT-II",
  "PollIntervalSeconds": 2
}
```

Restrinja o acesso ao arquivo à conta que roda o serviço (ele contém a chave).

## 4. Página de teste (acentos)

```powershell
.\StorageStrategy.PrintAgent.exe --test-page
```

Saem 3 blocos (CP860, CP850, sem acentos). Escolha no Site, em "Acentos", a opção cujo bloco saiu correto.

## 5. Rodar

- **Console (para testar):** `.\StorageStrategy.PrintAgent.exe`
- **Serviço do Windows (PowerShell como administrador):**

```powershell
sc.exe create StorageStrategyPrintAgent binPath= "C:\StorageStrategy\PrintAgent\StorageStrategy.PrintAgent.exe" start= auto
sc.exe description StorageStrategyPrintAgent "Imprime as comandas do StorageStrategy na impressora térmica"
sc.exe failure StorageStrategyPrintAgent reset= 86400 actions= restart/5000/restart/5000/restart/30000
sc.exe start StorageStrategyPrintAgent
```

Em até 15 s a tela "Impressora do Caixa" no Site mostra **Conectada**.

> O serviço roda como *Local System* por padrão. Se a impressora foi instalada só para um usuário,
> configure o serviço para rodar com essa conta (`services.msc` → Fazer logon).

## 6. Logs

Visualizador de Eventos do Windows → *Logs do Windows → Aplicativo*, origem `StorageStrategyPrintAgent`.
A chave e o conteúdo do papel nunca são registrados.

## Problemas comuns

| Sintoma | Causa provável |
|---|---|
| "Chave da estação inválida" no log | Chave errada ou regenerada: copie a nova para o `appsettings.json` e reinicie o serviço |
| Site mostra "Desconectada" | Serviço parado, sem internet ou `ApiBaseUrl` errada |
| Job "Falhou: Impressora ... não encontrada" | `PrinterName` diferente do nome no Windows |
| Acentos trocados | Escolha outra opção em "Acentos" (use a página de teste) |
| Marcou "Impresso" mas não saiu papel | Alguns modelos não avisam o Windows quando acaba o papel. Confira a bobina |
