# Stage 0 (prepare image base)
FROM hapvidacorp.azurecr.io/hap/golden-image/dotnet-6-aspnet AS base

# Stage 1 (copy && restore layers from app)
FROM hapvidacorp.azurecr.io/hap/golden-image/dotnet-6-sdk AS build

ARG NUGET_USER
ARG NUGET_KEY
ARG SONAR_TOKEN
ARG SONAR_URL

WORKDIR /src
COPY *.sln ./
COPY *.config ./
COPY *.sonarqube ./.sonarqube/
COPY *.runsettings ./
COPY Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Api/Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Api.csproj/ Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Api/
COPY Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Domain/Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Domain.csproj/ Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Domain/
COPY Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Infra.Data.Queries/Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Infra.Data.Queries.csproj/ Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Infra.Data.Queries/
COPY Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.UnitTests/Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.UnitTests.csproj/ Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.UnitTests/

RUN rm nuget.config && mv nuget-production.config nuget.config
RUN sed -i 's,___NUGET_USER___,'$NUGET_USER',g' nuget.config
RUN sed -i 's,___NUGET_KEY___,'$NUGET_KEY',g' nuget.config
RUN dotnet tool restore && dotnet restore

COPY . .

# Stage 2 (run unit tests && code coverage)
LABEL test=true
RUN dotnet tool install dotnet-reportgenerator-globaltool  --tool-path /tools && \ 
    dotnet test --configuration Release  --nologo --results-directory /testresults --logger "trx;LogFileName=test_results.xml" /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=/testresults/coverage/ /p:Include="[*.Domain?]*"; exit 0 && \
    /tools/reportgenerator "-reports:/testresults/coverage/coverage.cobertura.xml" "-targetdir:/testresults/coverage/reports" "-reporttypes:HTMLInline;HTMLChart" 

# Stage 3 (build app)
WORKDIR /src/Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Api
RUN dotnet sonarscanner begin  \
    /k:"Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Api"  \ 
    /d:sonar.host.url=${SONAR_URL} \ 
    /d:sonar.login=${SONAR_TOKEN} \
    /d:sonar.scm.disabled=true && \
    dotnet build --configuration Release --nologo --no-restore && \
    dotnet sonarscanner end	/d:sonar.login=${SONAR_TOKEN}

# Stage 4 (publish release version)
FROM build AS publish
WORKDIR /src/Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Api
RUN dotnet publish --configuration Release --nologo --no-build -o /app/publish

# Stage 5 (generate final imagem)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Api.dll"]