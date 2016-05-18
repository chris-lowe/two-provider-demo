# two-provider-demo
Messaging with 2 third-party providers

This is an f5 runnable (from visual studio) demo showing messaging required to incorporate a primary and secondary third party with a manual switch over.

Implemented using the beta2 of NServiceBus v6 with Nhibernate/SQLExpress for persistence.

Use the console for the UI endpoint to send a command and see the messages flow through to the correect provider.

The next step would be to add a http listener for the providers to send acks which would send the complete message and complete the saga. You could have a timeout in the saga that automatically sent the request to the fail over provider if the ack didn't come back within a certain time
