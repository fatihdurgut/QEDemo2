export const environment = {
  production: true,
  apiGatewayUrl: 'https://api.pubs.example.com',
  apiUrls: {
    identity: 'https://identity.pubs.example.com',
    authors: 'https://authors.pubs.example.com',
    publishers: 'https://publishers.pubs.example.com',
    titles: 'https://titles.pubs.example.com',
    sales: 'https://sales.pubs.example.com',
    employees: 'https://employees.pubs.example.com',
    stores: 'https://stores.pubs.example.com',
    notifications: 'https://notifications.pubs.example.com',
    analytics: 'https://analytics.pubs.example.com'
  },
  signalRUrls: {
    notifications: 'https://notifications.pubs.example.com/hubs/notifications'
  }
};
