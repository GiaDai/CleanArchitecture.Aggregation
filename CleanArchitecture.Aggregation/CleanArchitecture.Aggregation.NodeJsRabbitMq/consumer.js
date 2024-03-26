const amqplib = require('amqplib');
const amqp_url = 'amqp://vietbank:123456789@localhost';

// Create connection function to RabbitMQ
const createConnection = async () => {
    try {
        const connection = await amqplib.connect(amqp_url);
        const channel = await connection.createChannel();
        return channel;
    } catch (error) {
        console.log(error);
    }
};

// Create consumer function to receive message from RabbitMQ
const consumeMessage = async (channel) => {
    try {
        await channel.assertQueue('queue');
        await channel.consume('queue', message => {
            console.log(JSON.parse(message.content.toString()));
        },{
            noAck: true
        });
    } catch (error) {
        console.log(error);
    }
};

// Create connection and receive message
createConnection().then(channel => {
    consumeMessage(channel);
});
