# Skype for Business Server Chatbot Connector

Connects an Azure Chatbot to Skype for Business **Server**

*Info: Microsoft Bot Framework supports Skype for Business __Online__ as one of its channel. Configure your chatbot in the Azure Portal, if you want to run it in Skype for Business Online*.

## Prerequisites

* An Azure Chatbot with a [Direct Line](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-channel-connect-directline?view=azure-bot-service-3.0) configuration
* A regular Skype for Business Server account with a license for binding the Azure Chatbot

## Get started

1. Install Visual Studio
2. Clone this GitHub repository
3. Open the cloned repository in Visual Studio
4. Edit the **app.config** file in the **sfb-server-chatbot-connector** project
  * Change **Tenant** to the tenant of your Skype for Business envoirment, which is typically in the form of *your-domain.com*
  * Change **SfbUsername** and **SfbPassword** to the credentials of your Skype for Business Server account
  * Change **DirectLineSecret** to the secret key in the Direct Line configuration of your Azure Chatbot

```xml
<add key="Tenant" value="REPLACEWITH_Tenant" />
<add key="SfbUsername" value="REPLACEWITH_SfbUsername" />
<add key="SfbPassword" value="REPLACEWITH_SfbPassword" />
<add key="DirectLineSecret" value="REPLACEWITH_DirectLineSecret" />
```

5. Run the application and start a conversation with the Chatbot in the Skype for Business client

![Chatbot running in Skype for Business Server](https://user-images.githubusercontent.com/28813834/40840810-7b0086b0-65a8-11e8-863b-1c6f6c1e87e4.PNG)

## Publish the application to Azure
1. Right click on the **sfb-server-chatbot-connector** project under the solution explorer in Visual Studio
2. Click on **Publish as Azure WebJob**
3. Choose a name for the WebJob and set the run mode to **Run Continuously**, otherwise the application won't work for a long time
4. Select **Microsoft Azure App Service** as the publish target and finish the publish configuration


## Limitations

* Rich text is not supported

Your contribution is highly welcomed

## Appendix
Special thanks to Kenichiro Nakamura and Tam Huynh on which work this application is based on:

https://github.com/kenakamu/UCWABot

https://github.com/kenakamu/ucwa2.0-cs

https://github.com/tamhinsf/ucwa-sfbo-console

### Disclaimer
This code is provided without any warranty of any kind

