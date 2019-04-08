using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BattleCruiser {
    public interface ISeaBoardController {
        void TileEnter(int x, int y);
        void TileExit(int x, int y);
        void TileClick(int x, int y);
        void TileRelease(int x, int y);
        UIElement GetDock();
        void SetSeaBoard(SeaBoard setTo);

        void OnConnect();
        void OnDisconnect();
    }
    public static class ISeaBoardControllerExtentions {
        public static void Attach(this ISeaBoardController controller, SeaBoard attachTo) {
            attachTo.tileClicked += controller.TileClick;
            attachTo.tileReleased += controller.TileRelease;
            attachTo.tileExit += controller.TileExit;
            attachTo.tileEnter += controller.TileEnter;
            attachTo.dock.Child = controller.GetDock();
            controller.SetSeaBoard(attachTo);
            controller.OnConnect();
        }
        public static void Detach(this ISeaBoardController controller, SeaBoard attachTo) {
            controller.OnDisconnect();
            attachTo.tileClicked -= controller.TileClick;
            attachTo.tileReleased -= controller.TileRelease;
            attachTo.tileExit -= controller.TileExit;
            attachTo.tileEnter -= controller.TileEnter;
            attachTo.dock.Child = null;
            controller.SetSeaBoard(null);
        }
    }
}
