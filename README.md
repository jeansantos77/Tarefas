EXPLICA��ES
-----------
1. Foi desenvolvido seguindo o padr�o Domain Driven Design
2. Foi usado o padr�o Repository
3. Na camada de testes tem uma pasta chamada "Cobertura dos Testes" com um html e uma imagem deste html monstrando o % de cobertura
4. Na camada de infra tem uma pasta Scripts contendo o script para os usu�rios


PERGUNTAS PARA O PO
-------------------

1. Qual o prazo para a entrega deste projeto?
2. Seria importante ter o usu�rio que criou e data da cria��o da tarefa?



MELHORIAS
---------
1. Dar uma refatorada no c�digo para inserir padroes de projetos
2. Desenvolver relat�rios que possam conter mais informa��es sobre as tarefas
3. Criar um conjunto de permiss�es e vincular aos usu�rios para permitir/bloquear acesso a determinadas funcionalidades
4. A medida que o software crescer pensar em usar arquitetura de microservi�os com mensageria
5. Melhorar o tratamento de erros
6. Adicionar logs
7. � medida que o software for crescendo otimizar os selects gerados pelo EF


DOCKER
------
# baixa a imagem do sqlserver
docker pull mcr.microsoft.com/mssql/server:2019-latest

# cria o container do sqlserver
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Senha@2023" -p 1450:1433 --name sqlserverdb -d mcr.microsoft.com/mssql/server:2019-latest

# cria a imagem da api
docker build -t image_apitarefas -f Dockerfile .

# executa o container baseado na imagem criada
docker run --name container_apitarefas -p 8000:80 image_apitarefas .

# executa o docker-compose
docker compose up --build


# Criei o arquivo docker-compose.yml para executar a api acessando o banco do container do sqlserver mas n�o tive sucesso
# Criei o container do sqlserver e da api mas n�o consegui rodar, localmente a api acessa o sqlserver do container mas a api rodando no container deu erro para acessar, pesquisando percebi
# que precisava criar o docker-compose mas deu erro na execu��o