// https://json-generator.com/
[
    '{{repeat(0, 70000)}}',
    {
        rate: '{{floating(0, 4000, 0, "0")}}',
        name: '{{firstName()}} {{surname()}}',
        barcode: '{{integer(100, 999)}}-{{integer(100, 999)}}-{{integer(100, 999)}}-{{integer(100, 999)}}-{{integer(100, 10000)}}',
        description: '{{lorem(1, "paragraphs")}}'
    }
]

// https://www.mockaroo.com/