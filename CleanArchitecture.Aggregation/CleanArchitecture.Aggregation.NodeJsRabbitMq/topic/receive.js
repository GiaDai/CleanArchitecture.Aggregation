const amqplib = require('amqplib');
const amqp_url = 'amqp://vietbank:123456789@localhost';

// create send message to exchance topic

const receiveMessage = async () => {
    // 1. Connect to RabbitMQ
    const connection = await amqplib.connect(amqp_url);
    // 2. Create channel
    const channel = await connection.createChannel();
    // 3. Create exchange
    const nameExchange = 'send_topic';
    await channel.assertExchange(nameExchange, 'topic',{
        durable: false
    });

    // 4. Create queue
    const {queue} = await channel.assertQueue('',{
        exclusive: true
    });

    // 5. Bind queue to exchange
    const args = process.argv.slice(2);
    if(!args.length){
        console.log('Usage: send.js [binding_key] [message]');
        process.exit(1);
    }

    /**
     * * co nghia la phu hop voi bat ky routing key nao
     * # co nghia la phu hop voi 0 hoac nhieu chu so
     * 
     */

    console.log(`waiting queue ${queue} ::: topic::${args}`);
    args.forEach(bindingKey => {
        channel.bindQueue(queue, nameExchange, bindingKey);
    });

    // 6. Consume message from queue
    await channel.consume(queue, message => {
        console.log(`[x] Received ${message.fields.routingKey} ${message.content.toString()}`);
    },{
        noAck: true
    });
}

receiveMessage();