import { observer } from "mobx-react-lite"
import { Link } from "react-router-dom"
import { Item, Label, List, Segment, Image } from "semantic-ui-react"

export default observer( function ActivityDetailedSidebar() {
    return (
        <>
            <Segment
                textAlign="center"
                style={{border:'none'}}
                attached='top'
                secondary
                inverted
                color="teal"
            >
                3 People Giong
            </Segment>
            <Segment attached>
                <List relaxed divided>
                    <Item style={{position: 'relative'}}>
                        <Label
                            style={{position: 'absolute'}}
                            color='orange'
                            ribbon='right'
                        >
                            Host
                        </Label>
                        <Image size="tiny" src={'/assets/user.png'} />
                        <Item.Content verticalAlign="middle">
                            <Item.Header as='h3'>
                                <Link to={'#'}>Bob</Link>
                            </Item.Header>
                            <Item.Extra style={{ color: 'orange'}}>Following</Item.Extra>
                        </Item.Content>
                    </Item>

                    <Item style={{position: 'relative'}}>
                        <Image size="tiny" src={'/assets/user.png'} />
                        <Item.Content verticalAlign="middle">
                            <Item.Header as='h3'>
                                <Link to={"#"}>Tom</Link>
                            </Item.Header>
                            <Item.Extra style={{ color: 'orange'}}>Following</Item.Extra>
                        </Item.Content>
                    </Item>

                    <Item style={{position: 'relative'}}>
                        <Image size="tiny" src={'/assets/user.png'} />
                        <Item.Content verticalAlign="middle">
                            <Item.Header as='h3'>
                                <Link to={"#"}>Sally</Link>
                            </Item.Header>
                        </Item.Content>
                    </Item>
                </List>
            </Segment>
        </>
    )
})