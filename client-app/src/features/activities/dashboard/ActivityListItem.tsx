import { Link } from "react-router-dom";
import { Button, Icon, Item, Label, Segment } from "semantic-ui-react";
import { Activity } from "../../../app/models/activity";
import { format } from "date-fns";
import ActivityListItemAttendee from "./ActivityListItemAttendee";

interface Props {
  act: Activity;
}
const ActivityListItem = ({ act }: Props) => {
  return (
    <Segment.Group>
      <Segment>
        {act.isCancelled && (
          <Label
            attached="top"
            color="red"
            content="Cancelled"
            style={{ textAlign: "center" }}
          />
        )}
        <Item.Group>
          <Item>
            <Item.Image
              size="tiny"
              circular
              src={act.host?.image || "/assets/user.png"}
              style={{ marginBottom: 4 }}
            />
            <Item.Content>
              <Item.Header as={Link} to={`/activities/${act.id}`}>
                {act.title}
              </Item.Header>
              <Item.Description>
                Hosted by <Link to={`/profiles/${act.host?.username}`}>{act.host?.displayName}</Link>
              </Item.Description>
              {act.isHost && (
                <Item.Description>
                  <Label basic color="orange">
                    You are hosting this activity
                  </Label>
                </Item.Description>
              )}
              {act.isGoing && !act.isHost && (
                <Item.Description>
                  <Label basic color="green">
                    You are going to this activity
                  </Label>
                </Item.Description>
              )}
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>
      <Segment>
        <span>
          <Icon name="clock" /> {format(act.date!, "dd MMM yyyy h:mm aa")}
          <Icon name="marker" /> {act.venue}
        </span>
      </Segment>
      <Segment secondary>
        <ActivityListItemAttendee attendees={act.attendees} />
      </Segment>
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
