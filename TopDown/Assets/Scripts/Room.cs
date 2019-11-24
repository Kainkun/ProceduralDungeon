using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomData
{
    public uint layerNumber;
    public uint roomNumber;

    public roomData(uint layernum, uint roomnum)
    {
        layerNumber = layernum;
        roomNumber = roomnum;
    }

    public roomData goLeft(roomData room)
    {
        if (room.layerNumber >= 20)
            return null;

        roomData nextRoom = new roomData(room.layerNumber + 1, room.roomNumber * 3);

        return nextRoom;
    }

    public  roomData goMiddle(roomData room)
    {
        if (room.layerNumber >= 20)
            return null;

        roomData nextRoom = new roomData(room.layerNumber + 1, room.roomNumber * 3 + 1);

        return nextRoom;
    }

    public roomData goRight(roomData room)
    {
        if (room.layerNumber >= 20)
            return null;

        roomData nextRoom = new roomData(room.layerNumber + 1, room.roomNumber * 3 + 2);

        return nextRoom;
    }

    public roomData goBack(roomData room)
    {
        if (room.layerNumber <= 0)
            return null;

        roomData nextRoom = new roomData(room.layerNumber - 1, room.roomNumber / 3);

        return nextRoom;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashcode = 23;
            hashcode = (hashcode * 37) + (int)layerNumber;
            hashcode = (hashcode * 37) + (int)roomNumber;
            return hashcode;
        }
    }
}
