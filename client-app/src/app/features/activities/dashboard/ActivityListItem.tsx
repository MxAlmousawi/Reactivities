import { Link } from "react-router-dom";
import { Button, Icon, Item, Segment } from "semantic-ui-react";
import { Activity } from "../../../models/activity";

interface Props {
  act: Activity;
}
const ActivityListItem = ({ act }: Props) => {


  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Item.Image size="tiny" circular src="/assets/user.png" />
            <Item.Content>
              <Item.Header as={Link} to={`/activities/${act.id}`}>
                {act.title}
              </Item.Header>
              <Item.Description>Hosted bu Bob</Item.Description>
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>
      <Segment>
        <span>
          <Icon name="clock" /> {act.date}
          <Icon name="marker" /> {act.venue}
        </span>
      </Segment>
      <Segment secondary>Attendees go here</Segment>
      <Segment clearing>
        <span>
          {act.description}
          <Button
            as={Link}
            to={`/activities/${act.id}`}
            color="teal"
            floated="right"
            content="View"
          />
        </span>
      </Segment>
    </Segment.Group>
  );
};
export default ActivityListItem;
