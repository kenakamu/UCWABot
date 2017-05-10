# Looking for Skype for Business bot?
Now Microsoft BotFramework support Skype for Business as one of its channel. Use it for bot account!
See [Bot Framework Page](http://dev.botframework.com) for detail.

# UCWA 2.0 sample for BotFramework
This is a combined example of UCWA 2.0 and Bot Framework Direct Line.
- See [UCWA2.0-CS](https://github.com/kenakamu/ucwa2.0-cs) repo for UCWA SDK detail
- See [Direct Line v3](https://docs.botframework.com/en-us/restapi/directline3/#navtitle) page for Direct Line detail.

# What is this and how it works?
Let's say you have your bot logic up and running which is implemented by using Microsoft Bot Framework. And your organization uses Skype for Business. Then this sample helps you to connect these two.

You use UCWA 2.0 to sigin a user account into Skype for Business, and wait until someone talks to the account. When the account receives a message, it simply route the message to Bot Connector via Direct Line and let your code handles it. When the operation done, then it simply redirect the results to a user who asked the question. Simple, eh?

# Caution
There are something you need to understand (and welcome your contribution.)

1. It DOES NOT scale. UCWA 2.0 is for user level operation. You cannot run the code in multiple computers to scale out as they may receive same messages same time, and it may end up duplicates work.
2. This sample only handles text messages. It doesn't handle rich response such as attachments nor lists.

# How to start
[UCWA side]
See [UCWA2.0-CS](https://github.com/kenakamu/ucwa2.0-cs) repo how to setup.
[Bot Framework side]
See [BotFramework Homepage](https://docs.botframework.com/en-us/) for detail.

# License
MIT



