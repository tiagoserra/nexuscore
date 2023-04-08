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

    replaceInFile $pathUnitTestDestination'/Entities/'$EntityName'RegisterCommandHandler.cs' 'templates/AlterCommandHandlerUnitTests.txt'
    replaceInFile $pathUnitTestDestination'/Entities/'$EntityName'AlterCommandHandler.cs' 'templates/AlterCommandHandlerUnitTests.txt'
    replaceInFile $pathUnitTestDestination'/Entities/'$EntityName'RemoveCommandValidation.cs' 'templates/RemoveCommandHandlerUnitTests.txt'

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

    applicationHandler

}

applicationHandler() {

    echo 'Add in Application'
    pathApplicationDestination='../../Application/'$ModuleName
    pathApplicationCommands=$pathApplicationDestination'/Commands/'
    pathApplicationCommandValidations=$pathApplicationDestination'/CommandValidations/'
    pathApplicationCommandHandlers=$pathApplicationDestination'/CommandHandlers/'
    pathApplicationCommandQueries=$pathApplicationDestination'/CommandQueries/'
    pathApplicationInterfaces=$pathApplicationDestination'/Interfaces/'

    if [ ! -f "$pathApplicationDestination" ]; then
        mkdir -p $pathApplicationDestination
        mkdir -p $pathApplicationCommands
        mkdir -p $pathApplicationCommandValidations
        mkdir -p $pathApplicationCommandQueries
        mkdir -p $pathApplicationCommandHandlers
        mkdir -p $pathApplicationDestination'/EventHandlers/'
        mkdir -p $pathApplicationInterfaces
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

    replaceInFile $pathApplicationInterfaces'/I'$EntityName'Repository.cs' 'templates/IRepository.txt'

    infrastructureHandler

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
