# Mike May - August 2018
# despite its name this script will run a completely new sql server instance
# which will need to be populated with noter migrations.
#
# Look round for an existing instance using "docker ps -a" for one with the
# name "sql1" and then do "docker start sql1"

docker run -d --name sql1 -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=M1cromus' -e 'MSSQL_PID=Developer' -p 1401:1433 microsoft/mssql-server-linux:2017-latest