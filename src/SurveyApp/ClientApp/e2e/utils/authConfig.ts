export function getCredentials(): HTTPAuthCredentials {
  if (typeof process.env.DOMAIN !== 'undefined' && process.env.DOMAIN != null && process.env.DOMAIN !== '') {
    return {
      domain: process.env.DOMAIN,
      username: process.env.USERNAME,
      password: process.env.PASSWORD,
    };
  }

  const pos = process.env.USERNAME.indexOf('\\');
  if (pos === -1) {
    return {
      username: process.env.USERNAME,
      password: process.env.PASSWORD,
    };
  }

  const domain = process.env.USERNAME.substr(0, pos);
  const user = process.env.USERNAME.substr(pos + 1, process.env.USERNAME.length - pos - 1);
  return {
    domain,
    username: user,
    password: process.env.PASSWORD,
  };
}
