const amqplib = require('amqplib');
const amqp_url = 'amqp://vietbank:123456789@localhost';

const receiveMessage = async () => {
    try {
        // 1. Connect to RabbitMQ
        const connection = await amqplib.connect(amqp_url);
        // 2. Create channel
        const channel = await connection.createChannel();
        // 3. Create exchange
        await channel.assertExchange('video', 'fanout',{
            durable: false
        });
        // 4. Create queue
        const q = await channel.assertQueue('',{
            exclusive: true
        });
        // 5. Bind queue to exchange
        await channel.bindQueue(q.queue, 'video', '');
        // 6. Consume message from queue
        await channel.consume(q.queue, message => {
            console.log(`[x] Received ${message.content.toString()}`);
        },{
            noAck: true
        });
    } catch (error) {
        console.log(error);
    }
}

// Receive message
receiveMessage();