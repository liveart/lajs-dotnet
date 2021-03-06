Properties { # paths
  $MvcProject="$PSScriptRoot\..\..\LiveArt.WebAppMvc.Sample\LiveArt.WebAppMvc.Sample.csproj"
  $PublishTargetFolder="$PSScriptRoot\..\..\Published\wwwroot"
  $IISExpressExecutable="c:\Program Files (x86)\IIS Express\iisexpress.exe"
  $IISPort=8090
  $AppHostFile="$PSScriptRoot\..\..\Published\applicationhost.config"
}

Properties { #build
 $BuildConfiguration="Release" # or Debug
 $Verbosity="minimal" #[ValidateSet("quiet", "minimal", "normal", "detailed", "diagnostic")] 
}

Task Default -depends Publish,GenerateApplicationHost,RunIISExpress,StartBrowser

# Publish
Task Publish -depends CheckMSBuildVesion,Clean{
  Exec {
    $cmd="msbuild $MvcProject  /p:Configuration=$BuildConfiguration /verbosity:$Verbosity /p:DeployOnBuild=true /p:publishUrl=`"$PublishTargetFolder`" /p:PublishProfile=$PSScriptRoot\LocalFS.pubxml /p:RestorePackages=false /nr:false" 
     Write-Verbose $cmd
    Invoke-Expression $cmd
  }

  #fix publihising bug: restore Global.asax from mvc proejct, not webapi
  $mvcProjectFolder=(Split-Path $MvcProject -Parent)
  Copy-Item  $mvcProjectFolder\Global.asax $PublishTargetFolder
}

Task Clean {
 if(Test-Path $PublishTargetFolder) { #if exist remove all content before publish
   Get-ChildItem $PublishTargetFolder | Remove-Item -Recurse -Force
 } else {
   mkdir $PublishTargetFolder | Out-Null
 }
 
}


Task CheckMSBuildVesion {
  Exec {
      $output = &msbuild /version 2>&1
      $output=$output | Join-String
      Write-Host "$output" -ForegroundColor Yellow
      Assert ($output -Like "*14.0*") "'$output' should contain 14.0"
      Assert ($output -NotLike "*2.0*") "'$output' should not contain 2.0"
  }
 
}

Task GenerateApplicationHost {
  $sourceTemplate="$PSScriptRoot\applicationHost.config"
  #"<!-- Autogenerated. Don't modify manualy. -->" | Out-File  $AppHostFile

  $content=Get-Content  $sourceTemplate -Raw
  $content=$content.Replace("{WebAppMvcPath}",(Resolve-Path $PublishTargetFolder).Path)
  $content=$content.Replace("{WebAppMvcPort}",$IISPort)

  $parent=Resolve-Path $PublishTargetFolder | Split-Path -Parent
  $logsFolder="$parent\Logs"
  $TraceLogFilesFolder="$parent\TraceLogFiles"

  $content=$content.Replace("{LogsPath}",$logsFolder)
  $content=$content.Replace("{TraceLogFilesPath}",$TraceLogFilesFolder)

  
  
  $content | Out-File -FilePath $AppHostFile -Encoding utf8

  #ensure folders used for logs exists
  ($logsFolder,$TraceLogFilesFolder) | %{
    if(-not(Test-Path $_)){New-Item -Path $_ -ItemType directory}
  }
}

# =========== IIS Express ==============================
Task RunIISExpress -precondition {PortAvailable $IISPort} -depends GenerateApplicationHost{
 Assert(Test-Path $IISExpressExecutable) "Can't find '$IISExpressExecutable'"
 $wwwRoot=(Resolve-Path $PublishTargetFolder).Path
 
 #&"$IISExpressExecutable"  /config:$AppHostFile /systray:true /site:LiveArt.WebAppMvc.Sample
 start -FilePath  $IISExpressExecutable -ArgumentList "/config:$AppHostFile /systray:true /site:LiveArt.WebAppMvc.Sample" -NoNewWindow -PassThru
 
 
}

Task StartBrowser {
  start "http:\\localhost:$IISPort"
}

function PortAvailable($port){
 $hasListener=[boolean](([Net.NetworkInformation.IPGlobalProperties]::GetIPGlobalProperties()).GetActiveTcpListeners()  |  Where Port -EQ $port)
 !$hasListener
}