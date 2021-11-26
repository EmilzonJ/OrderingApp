import { FC } from 'react';
import { Auth0Provider } from '@auth0/auth0-react';

export const Auth0ProviderWithHistory: FC = ({ children }: any) => {
  const domain: string = 'dev-7kvoea6t.us.auth0.com';
  const clientId: string = '3JLV8uWCi23gnrWkpUmBaYzp8FahJNMm';
  const audience: string = 'https://realtimeapp-api/';

  return (
    <Auth0Provider
      domain={domain}
      clientId={clientId}
      redirectUri={`${window.location.origin}`}
      audience={audience}
    >
      {children}
    </Auth0Provider>
  );
};
