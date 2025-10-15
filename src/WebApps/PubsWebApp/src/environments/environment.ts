export const environment = {
  production: false,
  apiGatewayUrl: 'http://localhost:5000',
  apiUrls: {
    identity: 'http://localhost:5007',
    authors: 'http://localhost:5001',
    publishers: 'http://localhost:5002',
    titles: 'http://localhost:5003',
    sales: 'http://localhost:5004',
    employees: 'http://localhost:5005',
    stores: 'http://localhost:5006',
    notifications: 'http://localhost:5008',
    analytics: 'http://localhost:5009'
  },
  signalRUrls: {
    notifications: 'http://localhost:5008/hubs/notifications'
  }
};
