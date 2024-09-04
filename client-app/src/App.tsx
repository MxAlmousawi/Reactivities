import axios from "axios";
import { useEffect, useState } from "react";
import { Header, List, ListItem } from "semantic-ui-react";

function App() {
  const [activities, setActivities] = useState([]);

  useEffect(() => {
    axios.get("http://localhost:5000/api/Activities").then((response) => {
      setActivities(response.data);
    });
  }, []);

  return (
    <div>
      <Header as="h2" icon="users" content="Reactivities" />
      <List>
        {activities.map((act: any) => {
          return <ListItem key={act.id}>{act.title}</ListItem>;
        })}
      </List>
    </div>
  );
}

export default App;
