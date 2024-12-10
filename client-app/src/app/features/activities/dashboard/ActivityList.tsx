import { Button, Item, Label, Segment } from "semantic-ui-react";
import { useState } from "react";
import { useStore } from "../../../stores/store";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";

const ActivityList = () => {
  const { activityStore } = useStore();
  const { deleteActivity, activitiesByDate, loading } = activityStore;

  const [target, setTarget] = useState("");

  function handleActivityDelete(e: any, id: string) {
    setTarget(e.target.name);
    deleteActivity(id);
  }

  return (
    <Segment>
      <Item.Group divided>
        {activitiesByDate.map((act) => (
          <Item key={act.id}>
            <Item.Content>
              <Item.Header as="a">{act.title}</Item.Header>
              <Item.Meta>{act.date}</Item.Meta>
              <Item.Description>
                <Item.Extra>
                  <Button
                  as={Link}
                  to={`/activities/${act.id}`}
                    floated="right"
                    content="View"
                    color="blue"
                  />
                  <Button
                    name={act.id}
                    floated="right"
                    content="Delete"
                    color="red"
                    onClick={(e) => {
                      handleActivityDelete(e, act.id);
                    }}
                    loading={loading && target === act.id}
                  />
                  <Label basic content={act.category} />
                </Item.Extra>
              </Item.Description>
            </Item.Content>
          </Item>
        ))}
      </Item.Group>
    </Segment>
  );
};
export default observer(ActivityList);
