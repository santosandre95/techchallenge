Techallenge grupo 26 - PosTech Arquetura dotNet com Azure

terceira entraga  microsserviços com mensageria via rabbitmq

para rodar o ambiente entrar na pasta da solução e rodar o comando docker-compose up que todo o ambiente será gerado abaixo segue a denfinção do composer do ambiente na sequencia de iniciação


  1 rabbitmq:
  2 sqlserver-data:
  3 api:
  4 add_service:
  5 buscadd_service:
  6 buscaid_service:
  7 buscatodos_service:
  8 delete_service:
  9 update_service:
  10 prometheus-data:
  11 grafana-data:

todas as aplicações tem comunicação com o banco, para não ter conflitos foi escolhido para o projeto da api inicar as micarssões do banco

