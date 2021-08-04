module.exports = {
  chainWebpack: config => {
    config
      .plugin('html')
      .tap(args => {
        args[0].title = 'Food Delivery Co';
        return args;
      });
  }
};
