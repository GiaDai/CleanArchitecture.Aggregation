const qmqplib = require('amqplib');
const amqp_url = 'amqp://vietbank:123456789@localhost';

const postVideo = async ({msg}) => {
    try {
        // 1. Connect to RabbitMQ
        const connection = await qmqplib.connect(amqp_url);
        // 2. Create channel
        const channel = await connection.createChannel();
        // 3. Create exchange
        await channel.assertExchange('video', 'fanout',{
            durable: false
        });
        // 4. Publish message to exchange
        await channel.publish('video', '', Buffer.from(JSON.stringify(msg)));
        console.log(`[x] Sent ${JSON.stringify(msg)}`);

        setTimeout(() => {
            connection.close();
            process.exit(0);
        }, 2000);
    } catch (error) {
        console.log(error);
    }
}

const msg = {
    name: process.argv.slice(2).join(' ') || 'John Doe'
};

postVideo({msg});