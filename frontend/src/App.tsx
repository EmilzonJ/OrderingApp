import { Switch } from 'react-router';
import './App.css';
import ProtectedRoute from './Auth0/ProtectedRoute';
import { Products } from './features/Products';

function App() {
  return (
    <Switch>
      <ProtectedRoute path='/' component={Products} />
    </Switch>
  );
}

export default App;
