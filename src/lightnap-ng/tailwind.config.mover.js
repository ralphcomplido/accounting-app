const fs = require('fs');
const path = require('path');

const src = path.join(__dirname, 'tailwind.config.js');
const dest = path.join(__dirname, 'tailwind.config.js.bak');

const action = process.argv[2];

if (action === 'hide') {
  if (fs.existsSync(src)) {
    fs.renameSync(src, dest);
    console.log('tailwind.config.js hidden');
  }
} else if (action === 'restore') {
  if (fs.existsSync(dest)) {
    fs.renameSync(dest, src);
    console.log('tailwind.config.js restored');
  }
}
