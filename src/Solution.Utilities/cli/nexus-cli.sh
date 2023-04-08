#!/bin/bash
#----------------------------------------------------------------------------------------------------------------------------------------
#   Script: nexus-cli.sh
#   Author: Tiago Serra
#   Date: 08/04/2023 09:00
#   Summary: Script to create files in layers
#   Comments: how to call script ./nexus-cli.sh ModuleName EntityName yes no
#   Tips: chmod +x nexus-cli.sh
#
#   ModuleName => module
#   EntityName => entity
#   yes => to create unit tests
#   yes => to create seed
#
#----------------------------------------------------------------------------------------------------------------------------------------

ModuleName=$1
EntityName=$2
WithUnitTest=$3

if [[ -z "$ModuleName" ]]; then
    echo ERROR: "argument error (ModuleName) "
    exit -1
fi

if [[ -z "$EntityName" ]]; then
    echo ERROR: "argument error (EntityName) "
    exit -1
fi

replace() {
    sed -i '' "s/$1/$2/g" $3
}

replaceContentByReplaceIfNotExists() {

    result=$(cat $3 | grep -c "$4")

    if [ $result -eq 0 ]; then
        sed -i '' "s|$1|$2|g" $3
    fi
}

replaceInFile() {

    pathFileDestination=$1
    template=$2

    cp $template $pathFileDestination
    replace '%##%' $EntityName $pathFileDestination
    replace '%#MODULE#%' $ModuleName $pathFileDestination
    replace '%#GROUP#%' $EntityName's' $pathFileDestination
    replace '%#table#%' $EntityName $pathFileDestination
    replace '%#lower#%' $(echo $EntityName | tr '[:upper:]' '[:lower:]') $pathFileDestination
    replace '%#schema#%' $ModuleName $pathFileDestination
    replace '%#module_lower#%' $(echo $ModuleName | tr '[:upper:]' '[:lower:]') $pathFileDestination
}

unitTestsHandler(){

    echo 'Add Unit Tests'
    pathUnitTestDestination='../../../tst/UnitTests/Domain/'$ModuleName

    if [ ! -f "$pathUnitTestDestination" ]; then
        mkdir -p $pathUnitTestDestination
        mkdir -p $pathUnitTestDestination'/Entities/'
    fi

    replaceInFile $pathUnitTestDestination'/Entities/'$EntityName'UnitTests.cs' 'templates/EntityUnitTest.txt'

}

domainHandler() {

    echo 'Add in Domain'
    pathDomainDestination='../../Domain/'$ModuleName

    if [ ! -f "$pathDomainDestination" ]; then
        mkdir -p $pathDomainDestination
        mkdir -p $pathDomainDestination'/Entities/'
        mkdir -p $pathDomainDestination'/Enums/'
        mkdir -p $pathDomainDestination'/Events/'
        mkdir -p $pathDomainDestination'/Permissions/'
    fi

    pathPermissions=$pathDomainDestination'/Permissions/'

    replaceInFile $pathDomainDestination'/Entities/'$EntityName'.cs' 'templates/Entity.txt'
    replaceInFile $pathPermissions'/'$EntityName'Permission.cs' 'templates/Permission.txt'

    if [ $WithUnitTest = 'yes' ]; then
        unitTestsHandler
    fi

}
