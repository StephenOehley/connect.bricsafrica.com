<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="WebRole.Azure" generation="1" functional="0" release="0" Id="5887030d-9efc-4b9b-8a1c-15b3ba7d2274" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
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
      </componentports>
      <settings>
        <aCS name="Certificate|WebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/WebRole.Azure/WebRole.AzureGroup/MapCertificate|WebRole:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
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
          <role name="WebRole" generation="1" functional="0" release="0" software="C:\Workspace\CrystalTouch\MatchMaker\BricsWeb\Web\WebRole.Azure\csx\Debug\roles\WebRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="3584" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
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
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheArbitrationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheClusterPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheReplicationPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheServicePortInternal&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.Caching.cacheSocketPort&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
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
    <implementation Id="25d76c6b-919f-4f96-a9a9-9e17a7bb69a2" ref="Microsoft.RedDog.Contract\ServiceContract\WebRole.AzureContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="66adfd32-ebb2-4fea-972a-036d996269fb" ref="Microsoft.RedDog.Contract\Interface\WebRole:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="2b0cf17c-4b61-402a-8a1a-8b9f5106bcb3" ref="Microsoft.RedDog.Contract\Interface\WebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/WebRole.Azure/WebRole.AzureGroup/WebRole:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>