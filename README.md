 Sistema de Testes Automatizados

Este projeto é um sistema de testes automatizados utilizado para controlar a comunicação e realizar testes de dispositivos (como placas de circuito impresso - PCBs) via comunicação serial. Ele inclui funcionalidades como leitura de dados do dispositivo, execução de testes, geração de logs e envio de informações para um servidor remoto.

## Funcionalidades

- **Controle via porta serial**: Comunicação com dispositivos usando portas seriais, enviando comandos e recebendo dados.
- **Testes de dispositivos**: Realização de testes para verificar se os valores medidos pelos dispositivos estão dentro dos intervalos esperados.
- **Interface gráfica**: Interface de usuário com o Windows Forms para controle do processo de testes e visualização dos resultados.
- **Geração de logs**: Criação de logs detalhados em formato JSON com os resultados dos testes, incluindo informações como nome do teste, resultado e dados de tempo.
- **Envio de logs para servidor**: Os logs gerados são enviados para um servidor remoto para armazenamento e análise.

## Requisitos

- **.NET Framework 4.7.2** ou superior.
- **Bibliotecas**:
  - `Newtonsoft.Json` para manipulação de JSON.
  - `System.IO.Ports` para comunicação serial.
  - `System.Net.Http` para enviar dados para um servidor remoto.
  
