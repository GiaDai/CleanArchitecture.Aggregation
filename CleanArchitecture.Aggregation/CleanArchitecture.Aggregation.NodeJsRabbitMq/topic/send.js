const amqplib = require('amqplib');
const amqp_url = 'amqp://vietbank:123456789@localhost';

// create send message to exchance topic

const sendMessage = async () => {
    // 1. Connect to RabbitMQ
    const connection = await amqplib.connect(amqp_url);
    // 2. Create channel
    const channel = await connection.createChannel();
    // 3. Create exchange
    const nameExchange = 'send_topic';
    await channel.assertExchange(nameExchange, 'topic',{
        durable: false
    });


    const args = process.argv.slice(2);
    const msg = args[1] || 'Hello World';
    const topic = args[0] || 'anonymous.info';

    console.log(`[x] Sent ${topic} ${msg}`);

    // 4. Publish message to exchange
    await channel.publish(nameExchange, topic, Buffer.from(msg));

    console.log(`[x] Sent ${topic} ${msg} OK`);

    setTimeout(() => {
        connection.close();
        process.exit(0);
    }, 2000);
}

sendMessage();