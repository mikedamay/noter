docker run -d --name sql1 -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=M1cromus' -e 'MSSQL_PID=Developer' -p 1401:1433 microsoft/mssql-server-linux:2017-latest