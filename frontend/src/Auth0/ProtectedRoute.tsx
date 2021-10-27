import { withAuthenticationRequired } from '@auth0/auth0-react';
import { Route } from 'react-router-dom';

const ProtectedRoute = ({ component, ...args }: any) => (
  <Route component={withAuthenticationRequired(component)} {...args} />
);

export default ProtectedRoute;
