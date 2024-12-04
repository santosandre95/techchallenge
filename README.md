Techallenge grupo 26 - PosTech Arquetura dotNet com Azure

quarta entrega entraga  Orquestração de Containers e Elasticsearch


para rodar o ambiente entrar na pasta da solução e rodar  iniciar os pv pvc services pods e deployments  na pasta kiubernete que todo o ambiente será gerado abaixo segue a denfinção  do ambiente na sequencia de iniciação

antes de iniciar o projeto necessita de algumas alterações conforme abaixo.

o arquivo credentials  no campo data do dockerconfigjson gerar a credencial de acesso ao docker convertida para base 64
![image](https://github.com/user-attachments/assets/187621b2-5554-499b-8030-d08396b5c7b8)

no arquivo pv informar no path o caminho para gerar o volume na pasta onde está o projeto e dar acesso e permissão de edição neles

![image](https://github.com/user-attachments/assets/b811b48d-7d5a-4164-b2cf-1390f3c49035)

após feitas essas alterações iniciar ynmls na sequencia abaixo
  1 congimap :
  2 sercrets:
  3 Credencial:
  4 PV:
  5 PVC:
  6 external-deployments:
  7 internal-deployments:
  8 services:


todas as aplicações tem comunicação com o banco, para não ter conflitos foi escolhido para o projeto da api inicar as migarssões do banco

