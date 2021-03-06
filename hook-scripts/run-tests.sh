#!/bin/bash
protected_branch='master'
current_branch=$(git symbolic-ref HEAD | sed -e 's,.*/\(.*\),\1,')
RED='\033[0;31m'
GREEN='\033[1;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# only run this if you are pushing to master
if [[ $current_branch = $protected_branch ]] ; then
    echo -e "${YELLOW}Running pre push to master check...${NC}"

    echo -e "${YELLOW}Trying to build tests project...${NC}"
    
    #Let's speed things up a little bit
    DOTNET_CLI_TELEMETRY_OPTOUT=1
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
    
    # build the project
    dotnet build

    # $? is a shell variable which stores the return code from what we just ran
    rc=$?
    if [[ $rc != 0 ]] ; then
        echo -e "${RED}Failed to build the project, please fix this and push again${NC}"
        echo ""
        exit $rc
    fi

    # navigate to the test project to run the tests
    # TODO: change this to your test project directory
    cd PrismTest

    echo -e "${YELLOW}Running unit tests...${NC}"
    echo ""

    # run the unit tests
    dotnet test

    # $? is a shell variable which stores the return code from what we just ran
    rc=$?
    if [[ $rc != 0 ]] ; then
        # A non-zero return code means an error occurred, so tell the user and exit
        echo -e "${RED}Unit tests failed, please fix and push again${NC}"
        echo ""
        exit $rc
    fi

    # Everything went OK so we can exit with a zero
    echo -e "${GREEN}Pre push check passed!${NC}"
    echo ""
fi

exit 0