#Welcome to the Vontobel.Middleware.IBT wiki!

The solution to ensure the message delivery from a Source end-point to destination end-point. 

**Key Features**
* **Extendable Source & Sink:** Future implementations can be added without altering the framework.
* **Ensure Message Delivery** All the messages and subscriptions are storied in Database. Once the message is delivered to particular Partner, the system ensure it wont be sent again. However if required can be pushed again.

**Architecture Diagram**
![](https://github.com/talha-liaquat/Vontobel.Middleware.IBT/blob/master/ArchitectureDiagram.png)

Future:
* Add logging framework e.g. log4net
* Implementation of different end points e.g. MSMQ, IBM MQ, Restful Services etc
* To configure multiple sources & sinks via configuration file or database.
