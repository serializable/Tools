## Convert-WithXsl.ps1
## check  parameters
param(
	[string] $in=$(throw "Please specify a file containing the XML input."), 
	[string] $by=$(throw "Please specify a file containing the XSLT."), 
	[string] $out=$(throw "Please specify a file to contain the XML output.")
	)


## original by  Joel 'Jaykul' Bennett
## taken from http://huddledmasses.org/convert-xml-with-xslt-in-powershell/
function Convert-WithXslt($originalXmlFilePath, $xslFilePath, $outputFilePath) 

{
   ## Simplistic error handling
   $xslFilePath = resolve-path $xslFilePath
   if( -not (test-path $xslFilePath) ) { throw "Can't find the XSL file" } 

   $originalXmlFilePath = resolve-path $originalXmlFilePath
   if( -not (test-path $originalXmlFilePath) ) { throw "Can't find the XML file" } 

#   $outputFilePath = resolve-path $outputFilePath
#   if( -not (test-path (split-path $outputFilePath)) ) { throw "Can't find the output folder" } 

   ## Get an XSL Transform object (try for the new .Net 3.5 version first)
   $EAP = $ErrorActionPreference
   $ErrorActionPreference = "SilentlyContinue"

   $xslt = new-object system.xml.xsl.xslcompiledtransform  
   trap [System.Management.Automation.PSArgumentException] 
   {  # no 3.5, use the slower 2.0 one
      $ErrorActionPreference = $EAP
      $xslt = new-object system.xml.xsl.xsltransform
   }
   $ErrorActionPreference = $EAP

   ## load xslt file
   $xslt.Load( $xslFilePath )

   ## transform 
   $xslt.Transform( $originalXmlFilePath, $outputFilePath )
}


## set path
[Environment]::CurrentDirectory=(Get-Location -PSProvider FileSystem).ProviderPath

Convert-WithXslt $in $by $out



