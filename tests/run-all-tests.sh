#!/bin/bash
# run-all-tests.sh
# Script to run all tests with coverage reporting

set -e

echo "======================================"
echo "Running All Tests with Coverage"
echo "======================================"
echo ""

# Colors for output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Create test results directory
mkdir -p TestResults

echo -e "${YELLOW}Step 1: Running Unit Tests...${NC}"
dotnet test --filter "FullyQualifiedName~UnitTests" \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults \
  --logger "trx;LogFileName=unit-tests.trx" \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=cobertura \
  /p:CoverletOutput=./TestResults/unit-coverage.cobertura.xml \
  /p:Threshold=85 \
  /p:ThresholdType=line \
  /p:ThresholdStat=total

if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Unit tests passed${NC}"
else
    echo -e "${RED}✗ Unit tests failed${NC}"
    exit 1
fi

echo ""
echo -e "${YELLOW}Step 2: Running Integration Tests...${NC}"
dotnet test --filter "FullyQualifiedName~IntegrationTests" \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults \
  --logger "trx;LogFileName=integration-tests.trx" \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=cobertura \
  /p:CoverletOutput=./TestResults/integration-coverage.cobertura.xml

if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Integration tests passed${NC}"
else
    echo -e "${RED}✗ Integration tests failed${NC}"
    exit 1
fi

echo ""
echo -e "${GREEN}======================================"
echo "All Tests Passed Successfully!"
echo "======================================${NC}"
echo ""
echo "Test results available in: ./TestResults"
echo "Coverage reports:"
echo "  - Unit Tests: ./TestResults/unit-coverage.cobertura.xml"
echo "  - Integration Tests: ./TestResults/integration-coverage.cobertura.xml"
