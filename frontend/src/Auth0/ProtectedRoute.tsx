import { withAuthenticationRequired } from '@auth0/auth0-react';
import { FC } from 'react';
import { Route, RouteProps } from 'react-router-dom';

interface IProtectedRoute extends RouteProps {
  component: FC<{}>;
}

const ProtectedRoute = ({ component, ...args }: IProtectedRoute) => (
  <Route component={withAuthenticationRequired(component)} {...args} />
);

export default ProtectedRoute;
