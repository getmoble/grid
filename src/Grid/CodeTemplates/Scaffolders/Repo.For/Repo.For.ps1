[T4Scaffolding.Scaffolder(Description = "Enter a description of Repository.For here")][CmdletBinding()]
param(   
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,     
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

##############################################################
# NAMESPACE
##############################################################
$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
$rootNamespace = $namespace
$dotIX = $namespace.LastIndexOf('.')
if($dotIX -gt 0){
	$rootNamespace = $namespace.Substring(0,$namespace.LastIndexOf('.'))
}

##############################################################
# Project Name
##############################################################
$DALProjectName = $rootNamespace + ".DAL"
$DALProject = Get-Project $DALProjectName

##############################################################
# Add Data Repository - ModelType
##############################################################
$outputPath = $ModelType + "Repository"
$namespace = $DALProjectName
$ximports = "Grid.DAL.Interfaces"

Add-ProjectItemViaTemplate $outputPath -Template Repository `
	-Model @{ Namespace = $namespace; ClassName = $ModelType; ExtraUsings = $ximports } `
	-SuccessMessage "Added Repository at {0}" `
	-TemplateFolders $TemplateFolders -Project $DALProjectName -CodeLanguage $CodeLanguage -Force:$Force

try{
	$file = Get-ProjectItem "$($outputPath).cs" -Project $DALProjectName
	$file.Open()
	$file.Document.Activate()	
	$DTE.ExecuteCommand("Edit.FormatDocument", "")
	$DTE.ActiveDocument.Save()
}catch {
	Write-Host "Hey, you better not be clicking around in VS while we generate code" -ForegroundColor DarkRed
}

##############################################################
# create repository interface for modeltype
##############################################################
$outputPath = "I" + $ModelType + "Repository"
$namespace = $DALProject + ".Interfaces.Data"
$ximports = "Grid.DAL.Interfaces"

Add-ProjectItemViaTemplate $outputPath -Template IRepository `
	-Model @{ Namespace = $namespace; ClassName = $ModelType; ExtraUsings = $ximports } `
	-SuccessMessage "Added IRepository of $ModelType output at {0}" `
	-TemplateFolders $TemplateFolders -Project $DALProjectName -CodeLanguage $CodeLanguage -Force:$Force

try{
	$file = Get-ProjectItem "$($outputPath).cs" -Project $coreProjectName
	$file.Open()
	$file.Document.Activate()	
	$DTE.ExecuteCommand("Edit.FormatDocument", "")
	$DTE.ActiveDocument.Save()
}catch {
	Write-Host "Hey, you better not be clicking around in VS while we generate code" -ForegroundColor DarkRed
}

##############################################################
# Register the entity in the DbContext
##############################################################
$pluralName = Get-PluralizedWord $ModelType

$class = Get-ProjectType "DataContext" -Project $DALProjectName
$propertyToAdd = "public DbSet<" + $ModelType + "> " + $pluralName + "{ get; set; }"
$projPath = $DALProject.FullName.Replace($DALProjectName + '.csproj', 'DataContext.cs')
$checkForThis = "public DbSet<" + $ModelType + ">"
$propExists = $false
$file = $projPath
Get-Content $file | foreach-Object {  if($_.Contains($checkForThis)){ $propExists = $true }
}

if(!$propExists){	Add-ClassMember $class $propertyToAdd	}

try{
	$file = Get-ProjectItem "DataContext.cs" -Project $DALProjectName
	$file.Open()
	$file.Document.Activate()	
	$DTE.ExecuteCommand("Edit.FormatDocument", "")
	$DTE.ActiveDocument.Save()
}catch {
	Write-Host "Hey, you better not be clicking around in VS while we generate code" -ForegroundColor DarkRed
}