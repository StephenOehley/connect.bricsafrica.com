<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="WebRole.Azure" generation="1" functional="0" release="0" Id="5d924ef4-66a0-4c2c-8fd2-89e1e40b334b" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="WebRole.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="WebRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/WebRole.Azure/WebRole.AzureGroup/LB:WebRole:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="WebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/WebRole.Azure/WebRole.AzureGroup/LB:WebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
        <inPort name="WebRole:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/WebRole.Azure/WebRole.AzureGroup/LB:WebRole:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|WebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapCertificate|WebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="WebRole:CloudToolsDiagnosticAgentVersion" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRole:CloudToolsDiagnosticAgentVersion" />
          </maps>
        </aCS>
        <aCS name="WebRole:IntelliTrace.IntelliTraceConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRole:IntelliTrace.IntelliTraceConnectionString" />
          </maps>
        </aCS>
        <aCS name="WebRole:Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRole:Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" />
          </maps>
        </aCS>
        <aCS name="WebRole:Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRole:Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" />
          </maps>
        </aCS>
        <aCS name="WebRole:Microsoft.WindowsAzure.Plugins.Caching.Loglevel" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRole:Microsoft.WindowsAzure.Plugins.Caching.Loglevel" />
          </maps>
        </aCS>
        <aCS name="WebRole:Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRole:Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" />
          </maps>
        </aCS>
        <aCS name="WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="WebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="WebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="WebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="WebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="WebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapWebRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:WebRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:WebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:WebRole:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint">
          <toPorts>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort">
          <toPorts>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort">
          <toPorts>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort">
          <toPorts>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal">
          <toPorts>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort">
          <toPorts>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="SW:WebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|WebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapWebRole:CloudToolsDiagnosticAgentVersion" kind="Identity">
          <setting>
            <aCSMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/CloudToolsDiagnosticAgentVersion" />
          </setting>
        </map>
        <map name="MapWebRole:IntelliTrace.IntelliTraceConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/IntelliTrace.IntelliTraceConnectionString" />
          </setting>
        </map>
        <map name="MapWebRole:Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" kind="Identity">
          <setting>
            <aCSMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" />
          </setting>
        </map>
        <map name="MapWebRole:Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" />
          </setting>
        </map>
        <map name="MapWebRole:Microsoft.WindowsAzure.Plugins.Caching.Loglevel" kind="Identity">
          <setting>
            <aCSMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.Caching.Loglevel" />
          </setting>
        </map>
        <map name="MapWebRole:Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" kind="Identity">
          <setting>
            <aCSMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" />
          </setting>
        </map>
        <map name="MapWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapWebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapWebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapWebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WebRole" generation="1" functional="0" release="0" software="C:\Workspace\CrystalTouch\MatchMaker\BricsWeb\Web\WebRole.Azure\csx\Release\roles\WebRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="3584" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" protocol="tcp" portRanges="8172" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/WebRole.Azure/WebRole.AzureGroup/SW:WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort" />
                </outToChannel>
              </outPort>
              <outPort name="WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/WebRole.Azure/WebRole.AzureGroup/SW:WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort" />
                </outToChannel>
              </outPort>
              <outPort name="WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/WebRole.Azure/WebRole.AzureGroup/SW:WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort" />
                </outToChannel>
              </outPort>
              <outPort name="WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/WebRole.Azure/WebRole.AzureGroup/SW:WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal" />
                </outToChannel>
              </outPort>
              <outPort name="WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/WebRole.Azure/WebRole.AzureGroup/SW:WebRole:Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort" />
                </outToChannel>
              </outPort>
              <outPort name="WebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/WebRole.Azure/WebRole.AzureGroup/SW:WebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="CloudToolsDiagnosticAgentVersion" defaultValue="" />
              <aCS name="IntelliTrace.IntelliTraceConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.CacheSizePercentage" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.Loglevel" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.NamedCaches" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="Microsoft.WindowsAzure.Plugins.Caching.FileStore" defaultAmount="[1000,1000,1000]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRoleInstances" />
            <sCSPolicyFaultDomainMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="WebRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="WebRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="a7388966-6f57-4ea6-b4d3-4c831adbb1c4" ref="Microsoft.RedDog.Contract\ServiceContract\WebRole.AzureContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="30e89cfa-1ee8-4429-ad9f-69f3a431acff" ref="Microsoft.RedDog.Contract\Interface\WebRole:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="0f778180-81b8-4e01-84fc-036ead2175ae" ref="Microsoft.RedDog.Contract\Interface\WebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="94d632bf-9184-41f0-b338-72d1b5443fbe" ref="Microsoft.RedDog.Contract\Interface\WebRole:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole:Microsoft.WindowsAzure.Plugins.WebDeploy.InputEndpoint" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>