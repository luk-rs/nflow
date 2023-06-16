# Flow.Reactive (Work in Progress)
A framework for developing applications with the least boiler plate code possible through a reactive, functional-driven architecture.

## Core Concepts
### Distinguish between data and data transformation
WIP
### MicroServices
According to Wikipedia "*There is no single definition for microservices*" therefore is not easy to find a concrete sentence that fits all use cases, however we can all agree that usually it stands for a independent deployable unit.

### NanoServices
The idea behind these little fellows is to respect the Single Responsability Principle no matter what. They should have one and only one job in the system. 

**Flow.Reactive** already provides a complete set of the most **NanoServices** types used in an application (more on that below)  but you are free to create your own.
### Streams
Flow.Reactive considers 2 types of Streams that live inside a **MicroService**: **QueryableStreams** and **EventStreams**, both can be either **Public** - accessible to all  other **MicroServices** and to other application components (PresentationLayer for instance) or **Private** - accessible only by the **NanoServices** of that **MicroService**
#### QueryableStreams
A stream of this type contains always an initial value - under the hood is nothing more than a Rx BehaviorSubject

The value can only be changed due to a NanoService **data transformation**
#### EventStreams
Unlike the **QueryableStream**, the **EventStream** does not have an initial value, it represents something that happened (for instance: a system notification, a command that was sent, etc.)






> Written with [StackEdit](https://stackedit.io/).
