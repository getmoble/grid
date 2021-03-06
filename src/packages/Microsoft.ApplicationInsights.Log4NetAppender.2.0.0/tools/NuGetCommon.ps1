# Common logic for NuGet installation scripts

[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms") 

function ReportError
{
Param(
  [string]$message
 )
	throw $message
}

function ReportInfo([string] $message, [string] $caption)
{
	Write-Host $message -background yellow

	[System.Windows.Forms.MessageBox]::Show($message, $caption)
}

function ReportApplicationInsightsConfigNotFound()
{
	ReportInfo "Add Application Insights to your project to send log data to the Application Insights portal." "Can’t find ApplicationInsights.config"
}

function GetOrCreateElement
{
	Param(
 		[System.Xml.Linq.XContainer] $container,
 		[System.Xml.Linq.XName] $name)
        
	$element = $container.Element($name)
	if (!$element)
	{
		$element = New-Object -TypeName System.Xml.Linq.XElement -ArgumentList $name
    	$container.Add($element)
	}

	return $element
}

function ValidateProject
{
	Param([Object]$project)
	
	if(!$project)
	{
		ReportError "The Application Insights logging adapter package can’t determine the project you want to apply it to."
	}
}

function GetAIConfigPath
{
	Param([Object] $project)
	
	$aiConfigProjectItem = $project.ProjectItems | where {$_.Name -eq "ApplicationInsights.config"}

	$aiConfigPath = $aiConfigProjectItem.Properties | where {$_.Name -eq "LocalPath"}
	
	if(!$aiConfigPath)
	{
		throw "Can’t find ApplicationInsights.config. Add Application Insights to your project and retry."
	}

	return $aiConfigPath.Value
}

function GetWebConfigPath
{
	Param([object] $project)
	
	$webConfigProjectItem = $project.ProjectItems | where {$_.Name -eq "Web.config"}

	$webConfigPath = $webConfigProjectItem.Properties | where {$_.Name -eq "LocalPath"}

	if(!$webConfigPath)
	{
		throw "Can't find Web.config. Make sure you are targetting an appropriate project type."
	}
	
	return $webConfigPath.Value
}

function LoadXml
{
	Param([string] $filePath)
	
	$fileStream = New-Object -TypeName System.IO.FileStream -ArgumentList $filePath, "Open", "Read"
	
	try
	{
		$xml = [System.Xml.Linq.XElement]::Load($fileStream, "None");	#PreserveWhitespace
	}
	catch
	{
		ReportError "Couldn't load XML from " + $filePath + " " + $_
		return
	}
	finally
	{
		if($fileStream)
		{
			$fileStream.Dispose()
		}
	}
	
	return $xml
}

function GetInstrumentationKey
{
	Param([Object] $aiConfigPath)
	
	$xml = LoadXml $aiConfigPath
	
	$xmlns = [System.Xml.Linq.XNamespace]::Get("http://schemas.microsoft.com/ApplicationInsights/2013/Settings")
	$instrumentationKeyElement = $xml.Element($xmlns + "InstrumentationKey")

	if(!$instrumentationKeyElement)
	{
		ReportError "Can’t find the InstrumentationKey element in ApplicationInsights.config. Create an Application Insights resource in Microsoft Azure, then get a key from the Quick Start tile."
		return
	}

	return $instrumentationKeyElement.Value
}

function DoesAIConfigExist([Object] $project)
{
	$aiConfigProjectItem = $project.ProjectItems | where {$_.Name -eq "ApplicationInsights.config"}

	if(!$aiConfigProjectItem)
	{
		return $false
	}

	return $true
}

function CreateAttribute
{
	Param([String] $name, [Object] $value)
	
	return New-Object -TypeName System.Xml.Linq.XAttribute -ArgumentList $name, $value
}

function LoadWebConfig
{
	Param([Object] $project)
	
	$webConfigPath = GetWebConfigPath $project
	
	return LoadXml $webConfigPath
}

function SaveWebConfig
{
	Param([Object] $project, [Object] $xml)
	
	$filePath = GetWebConfigPath $project
	
	try
	{
		$xml.Save($filePath, "None");	#"DisableFormatting"
	}
	catch
	{
		ReportError "Couldn't write changes to web.config. Make sure that web.config is not open in Visual Studio and try again. " + $_
		return
	}
}

function RemoveIfNoChildren([System.Xml.Linq.XElement] $element)
{
	if([bool]$element.HasElements -eq $false)
	{
		$element.Remove()
	}
}