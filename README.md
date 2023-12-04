EXPLICA��ES
-----------
1. Foi desenvolvido seguindo o padr�o Domain Driven Design
2. Foi usado o padr�o Repository
3. Na camada de testes tem uma pasta chamada "Cobertura dos Testes" com um html e uma imagem deste html monstrando o % de cobertura
4. Na camada de infra tem uma pasta Scripts contendo o script para os usu�rios
5. Tem uma pasta Docker com uma imagem mostrando que foi executado do Docker


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
# foi criado um docker-compose.yml para criar o container do sqlserver e api
# depois foi executado as migrations para criar o banco

# executa o docker-compose
docker compose up --build

