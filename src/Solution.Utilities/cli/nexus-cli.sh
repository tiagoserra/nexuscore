#!/bin/bash
#----------------------------------------------------------------------------------------------------------------------------------------
#   Script: nexus-cli.sh
#   Author: Tiago Serra
#   Date: 08/04/2023 09:00
#   Summary: Script to create files in layers
#   Comments: how to call script ./nexus-cli.sh ModuleName EntityName yes yes
#   Tips: chmod +x nexus-cli.sh
#
#   ModuleName => module
#   EntityName => entity
#   yes => to create unit tests
#   yes => to create dump
#
#----------------------------------------------------------------------------------------------------------------------------------------

ModuleName=$1
EntityName=$2
WithUnitTest=$3
WithDump=$4

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
    replace '%#table#%' $EntityName $pathFileDestination
    replace '%#lower#%' $(echo $EntityName | tr '[:upper:]' '[:lower:]') $pathFileDestination
    replace '%#schema#%' $ModuleName $pathFileDestination
    replace '%#module_lower#%' $(echo $ModuleName | tr '[:upper:]' '[:lower:]') $pathFileDestination

}

unitTestsHandler() {

    echo 'Add Unit Tests'
    pathUnitTestDestination='../../../tst/UnitTests/Domain/'$ModuleName

    if [ ! -f "$pathUnitTestDestination" ]; then
        mkdir -p $pathUnitTestDestination
        mkdir -p $pathUnitTestDestination'/Entities/'
    fi

    replaceInFile $pathUnitTestDestination'/Entities/'$EntityName'UnitTests.cs' 'templates/EntityUnitTest.txt'

    pathUnitTestDestination='../../../tst/UnitTests/Application/'$ModuleName

    if [ ! -f "$pathUnitTestDestination" ]; then
        mkdir -p $pathUnitTestDestination
        mkdir -p $pathUnitTestDestination'/Commands/'
    fi

    replaceInFile $pathUnitTestDestination'/Commands/'$EntityName'RegisterCommandHandler.cs' 'templates/RegisterCommandHandlerUnitTests.txt'
    replaceInFile $pathUnitTestDestination'/Commands/'$EntityName'AlterCommandHandler.cs' 'templates/AlterCommandHandlerUnitTests.txt'
    replaceInFile $pathUnitTestDestination'/Commands/'$EntityName'RemoveCommandValidation.cs' 'templates/RemoveCommandHandlerUnitTests.txt'

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
        mkdir -p $pathDomainDestination'/Interfaces/'
    fi

    pathPermissions=$pathDomainDestination'/Permissions/'
    pathADomainInterfaces=$pathDomainDestination'/Interfaces/'

    replaceInFile $pathDomainDestination'/Entities/'$EntityName'.cs' 'templates/Entity.txt'
    replaceInFile $pathPermissions'/'$EntityName'Permission.cs' 'templates/Permission.txt'
    replaceInFile $pathADomainInterfaces'/I'$EntityName'Repository.cs' 'templates/IRepository.txt'

    applicationHandler

}

applicationHandler() {

    echo 'Add in Application'
    pathApplicationDestination='../../Application/'$ModuleName
    pathApplicationCommands=$pathApplicationDestination'/Commands/'
    pathApplicationCommandValidations=$pathApplicationDestination'/CommandValidations/'
    pathApplicationCommandHandlers=$pathApplicationDestination'/CommandHandlers/'
    pathApplicationCommandQueries=$pathApplicationDestination'/CommandQueries/'

    if [ ! -f "$pathApplicationDestination" ]; then
        mkdir -p $pathApplicationDestination
        mkdir -p $pathApplicationCommands
        mkdir -p $pathApplicationCommandValidations
        mkdir -p $pathApplicationCommandQueries
        mkdir -p $pathApplicationCommandHandlers
        mkdir -p $pathApplicationDestination'/EventHandlers/'
    fi

    replaceInFile $pathApplicationCommands'/'$EntityName'RegisterCommand.cs' 'templates/RegisterCommand.txt'
    replaceInFile $pathApplicationCommands'/'$EntityName'AlterCommand.cs' 'templates/AlterCommand.txt'
    replaceInFile $pathApplicationCommands'/'$EntityName'RemoveCommand.cs' 'templates/RemoveCommand.txt'

    replaceInFile $pathApplicationCommandValidations'/'$EntityName'RegisterCommandValidation.cs' 'templates/RegisterCommandValidation.txt'
    replaceInFile $pathApplicationCommandValidations'/'$EntityName'AlterCommandValidation.cs' 'templates/AlterCommandValidation.txt'
    replaceInFile $pathApplicationCommandValidations'/'$EntityName'RemoveCommandValidation.cs' 'templates/RemoveCommandValidation.txt'

    replaceInFile $pathApplicationCommandQueries'/'$EntityName'GetByIdCommandQuery.cs' 'templates/GetByIdCommandQuery.txt'
    replaceInFile $pathApplicationCommandQueries'/'$EntityName'GetAllWithPaginationCommandQuery.cs' 'templates/GetAllWithPaginationCommandQuery.txt'

    replaceInFile $pathApplicationCommandHandlers'/'$EntityName'RegisterCommandHandler.cs' 'templates/RegisterCommandHandler.txt'
    replaceInFile $pathApplicationCommandHandlers'/'$EntityName'AlterCommandHandler.cs' 'templates/AlterCommandHandler.txt'
    replaceInFile $pathApplicationCommandHandlers'/'$EntityName'RemoveCommandHandler.cs' 'templates/RemoveCommandHandler.txt'

    replaceInFile $pathApplicationCommandHandlers'/'$EntityName'GetByIdCommandQueryHandler.cs' 'templates/GetByIdCommandQueryHandler.txt'
    replaceInFile $pathApplicationCommandHandlers'/'$EntityName'GetAllWithPaginationCommandQueryHandler.cs' 'templates/GetAllWithPaginationCommandQueryHandler.txt'

    infrastructureHandler

    if [ $WithDump = 'yes' ]; then

        echo 'Add dump in Infrastructure Manager'

        pathDestination='../../Infrastructure.Manager/'$ModuleName
        pathDestinationDump=$pathDestination'/Dumps/'

        if [ ! -f "$pathDestination" ]; then
            mkdir -p $pathDestination
            mkdir -p $pathDestinationDump
        fi

        replaceInFile $pathDestinationDump'/'$EntityName'Dump.cs' 'templates/Dump.txt'
    fi

}

infrastructureHandler() {

    echo 'Add in Infrastructure'
    pathMappingDestination='../../Infrastructure/Data/Mappings/'$EntityName'Mapping.cs'
    replaceInFile $pathMappingDestination 'templates/Mapping.txt'

    pathRepositoryDestination='../../Infrastructure/Data/Repositories/'$EntityName'Repository.cs'
    replaceInFile $pathRepositoryDestination 'templates/Repository.txt'

    pathContextDestination="../../Infrastructure/Data/Contexts/SqlContext.cs"

    line='using Domain.'$ModuleName'.Entities;'
    replacerNewString=$line' \n//%#Domain#%'
    replaceContentByReplaceIfNotExists '//%#Domain#%' "$replacerNewString" $pathContextDestination "$line"

    line='public DbSet<'$EntityName'> '$EntityName's { get; set; }'
    replacerNewString=$line'  \n \t//%#DbSet#%'
    replaceContentByReplaceIfNotExists '//%#DbSet#%' "$replacerNewString" $pathContextDestination "$line"

}

domainHandler

if [ $WithUnitTest = 'yes' ]; then
    unitTestsHandler
fi
