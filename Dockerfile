FROM microsoft/dotnet:2.1-sdk

# Tag: clearhaus/dotnet

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        mono-runtime \
        libmono-system-core4.0-cil \
        libmono-system-xml-linq4.0-cil && \
    rm -rf /var/lib/apt/lists/*
