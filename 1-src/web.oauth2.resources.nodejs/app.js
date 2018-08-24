const express        = require('express');
const cors           = require('cors');
const authentication = require('./authentication');

const app = express();

// enable cors.
app.use(cors());

// enable authentication.
app.use(authentication);

// api.
app.get('/', function (req, res) {
  const json = {
    user: req.user,
    author: "lnhcode@outlook.com",
    github: "https://github.com/linianhui/oidc.example",
    text: "this is a node.js api.",
  };

  res.json(json);
});

app.listen(80);