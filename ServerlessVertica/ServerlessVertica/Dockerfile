FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base

RUN apt-get update
RUN apt-get -y install curl g++ make bzip2
RUN curl http://www.unixodbc.org/unixODBC-2.3.7.tar.gz | tar xz
WORKDIR unixODBC-2.3.7
RUN ./configure && make && make install
RUN echo "/usr/local/lib" >> /etc/ld.so.conf.d/x86_64-linux-gnu.conf
RUN ldconfig

WORKDIR /usr/local/lib
RUN ls
RUN mkdir -p /var/log/unixodbc

RUN mkdir -p /opt/vertica-odbc-driver
RUN cd /opt/vertica-odbc-driver && curl https://www.vertica.com/client_drivers/9.2.x/9.2.0-0/vertica-client-9.2.0-0.x86_64.tar.gz | tar xz
RUN cp /opt/vertica-odbc-driver/opt/vertica/include/* /usr/include

RUN echo "[VMart]" >> /usr/local/etc/odbc.ini
RUN echo "Description = Vertica Database using ODBC Driver" >> /usr/local/etc/odbc.ini
RUN echo "Driver = VerticaDSN" >> /usr/local/etc/odbc.ini

RUN echo "[Vertica]" >> /usr/local/etc/odbcinst.ini
RUN echo "Description = Vertica Database using ODBC Driver" >> /usr/local/etc/odbcinst.ini
RUN echo "Driver =/opt/vertica-odbc-driver/opt/vertica/lib64/libverticaodbc.so" >> /usr/local/etc/odbcinst.ini
RUN echo "Locale = en_US" >> /usr/local/etc/odbcinst.ini
RUN echo "[ODBC]" >> /usr/local/etc/odbcinst.ini
RUN echo "Threading = 1" >> /usr/local/etc/odbcinst.ini

RUN echo "[Driver]DriverManagerEncoding = UTF-16" >> /etc/vertica.ini
RUN echo "ODBCInstLib = /usr/local/lib/libodbcinst.so" >> /etc/vertica.ini
RUN echo "ErrorMessagesPath = /opt/vertica/lib64" >> /etc/vertica.ini
RUN echo "LogLevel = 4" >> /etc/vertica.ini
RUN echo "LogPath = /tmp" >> /etc/vertica.ini

ENV ODBCINI=/usr/local/etc/odbc.ini
ENV VERTICAINI=/etc/vertica.ini

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["ServerlessVertica/ServerlessVertica.csproj", "ServerlessVertica/"]
RUN dotnet restore "ServerlessVertica/ServerlessVertica.csproj"
COPY . .
WORKDIR "/src/ServerlessVertica"
RUN dotnet build "ServerlessVertica.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ServerlessVertica.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ServerlessVertica.dll"]