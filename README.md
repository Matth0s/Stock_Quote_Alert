# Stock Quote Alert

## Autor:
  - Nome: Matheus Moreira do Nascimento

## Introdução:

O programa monitora o valor de uma ação de preferência do usuário, comunicando-o por e-mail quando esse valor fica abaixo de um limite mínimo ou ultrapassa um limite máximo.

## Requisitos Para Compilação:

É necessário ter:
- .NET9.0
- pacote YahooFinanceApi - 2.3.3

## Como Buildar o Executavel:

Abra um terminal na pasta raiz do projeto e execute o seguinte comando:

**Comando para build:**
```cmd
dotnet publish --output . --runtime win-x64 --configuration Release --self-contained true -p:AssemblyName=stock-quote-alert -p:PublishSingleFile=true -p:DebugType=None
```
## Como Executar:

No diretório onde se encontra o executavel, crie um arquivo chamado `config.txt`, contendo as informações necessárias para conexão com o servidor SMTP, na seguinte ordem:
- E-mail do destinatário dos alertas
- Host do servidor SMTP
- Porta de acesso ao servidor
- E-mail do usuário (credencial de acesso)
- Senha do usuário

**Exemplo:**
```txt
receptor@dominio.com
smtp.dominio.com
587
usuario@dominio2.com
senha do usuario
```

Em seguida, execute o programa passando os seguintes parâmetros:
- Código da ação na B3 (este deve ser o **primeiro argumento**)
- Valor limite superior de monitoramento
- Valor limite inferior de monitoramento

**Exemplo:**
```cmd
stock-quote-alert.exe PETR4 22.67 22.59
```

## Como Interpretar:

O programa acompanha a cotação da ação em tempo real. Quando o valor da cotação ultrapassar o limite superior, um e-mail com o título **Alta** será enviado. Quando o valor da cotação cair abaixo do limite inferior, um e-mail com o título **Baixa** será enviado. Caso o valor da ação, após ultrapassar um dos limites, retorne para dentro do intervalo monitorado, um e-mail com o título **Estabilidade** será enviado, informando que o cenário do alerta anteriormente disparado não é mais válido.
