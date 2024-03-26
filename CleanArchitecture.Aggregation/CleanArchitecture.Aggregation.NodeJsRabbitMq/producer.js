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

// Create producer function to send message to RabbitMQ
const produceMessage = async (channel, message) => {
    try {
        await channel.assertQueue('queue',{
            durable: true
        
        });
        await channel.sendToQueue('queue', Buffer.from(JSON.stringify(message)),{
            expiration: '5000',
            persistent: true
        
        });
    } catch (error) {
        console.log(error);
    }
};

// Create message
const message = {
    id: 1,
    name: process.argv.slice(2).join(' ') || 'John Doe',
    age: 30
};

// Create connection and send message
createConnection().then(channel => {
    produceMessage(channel, message);
});