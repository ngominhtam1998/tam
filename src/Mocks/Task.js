const { v4: uuidv4 } = require('uuid');

let items = [
    {
        id : uuidv4(),
        name : "Abc hihi" ,
        level : 0,
    },
    {
        id : uuidv4(),
        name : "def" ,
        level : 1,
    },
    {
        id : uuidv4(),
        name : "1223",
        level : 2,
    },
]

export default items